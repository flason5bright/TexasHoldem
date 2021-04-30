// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Get the user name and store it to prepend to messages.
//var playername = prompt('Enter your name:', '');


//$.get(
//    "api/Room",
//    function (data) {
//        // var jsonData = $.parseJSON(data);
//        $("#roomList").html(" ");
//        for (var i = 0; i < data.length; i++) {
//            var html = '<li>房间名:' + data[i].name + '<button roomId="' + data[i].id + '" roomName="' + data[i].name + '"onclick="joinRoom(this)">加入</button> </li>'
//            $("#roomList").append(html);
//        }
//    },
//    "json"
//);

//}
//function prepare() {
//    connection.invoke("Prepare", data).then(function (data) {
//        alert(data);
//    });

// Set initial focus to message input box.
//var token = roomid + "&" + playername;


var connection = new signalR.HubConnectionBuilder()
    .withUrl('/gameHub')
    .build();

connection.start()
    .then(function () {
        //connection.invoke('AddPlayer', playername);
        console.log('connection started');

        //connection.invoke("JoinRoom", playername, roomid).then(function (data) {
        //    alert(data);
        //});
    })
    .catch(error => {
        console.error(error.message);
    });



//Vue.component('chip-panel', {
//    data: function () {
//        return {
//            betChips: []
//        }
//    },
//    props: {
//        chip: Object
//    },
//    computed: {
//        count: function () {
//            console.log(this.betChips);
//            return this.betChips.length;
//        }
//    },
//    methods: {
//        chipToggle: function () {
//            if (event.target.checked) {
//                this.$emit('chip-toggle', { 'chip': this.chip, 'isAdd': true })
//            }
//            else {
//                this.$emit('chip-toggle', { 'chip': this.chip, 'isAdd': false })

//            }
//        }
//    },
//    template: '#chips'
//})

Vue.component('chip-panel', {
    data: function () {
        return {
            betChips: []
        }
    },
    props: {
        player: Object
    },
    computed: {
        count: function () {
            console.log(this.betChips);
            return this.betChips.length;
        }
    },
    methods: {
        chipToggle: function () {
            if (event.target.checked) {
                this.$emit('chip-toggle', { 'chip': this.chip, 'isAdd': true })
            }
            else {
                this.$emit('chip-toggle', { 'chip': this.chip, 'isAdd': false })

            }
        }
    },
    template: '#chips'
})

Vue.component('riches', {
    data: function () {
        return {

        }
    },
    props: {
        player: Object
    },

    template: '#riches'
})

Vue.component('game-pool', {
    data: function () {
        return {

        }
    },
    props: {
        game: Object
    },
    template: '#gamePool'
})

Vue.component('gamer', {
    data: function () {
        return {
            addplayersrc: "/img/addplayer.png",
            canSelectSeat: true,
            roles: ["N/A", '庄', "小盲", "大盲"],
        }
    },
    props: {
        seat: Object,
        index: Number,
        self: Object,
        game: Object
    },
    methods: {
        selectSeat: function (index) {
            if (this.game) {
                if (this.game.gameStatus > 0 && this.game.gameStatus < 3) {
                    alert("游戏中不能换位置!");
                    return;
                }
            }
            connection.invoke('SelectSeat', index).then(function (data) {
                if (!data)
                    alert("该位置已经被别人选中!");
            });
        },
        getFirstImg: function () {
            if (this.game.isFinished)
                return this.seat.poker1.img;

            if (this.self.name == this.seat.name)
                return this.seat.poker1.img;
            return this.seat.poker1.backImg;
        },
        getSecondImg: function () {
            if (this.game.isFinished)
                return this.seat.poker2.img;

            if (this.self.name == this.seat.name)
                return this.seat.poker2.img;
            return this.seat.poker2.backImg;
        }
    },
    template: '#gamer'
})


var app = new Vue({
    el: '#app',
    data: {
        self: {},
        players: [],
        seats: new Array(9),
        addplayersrc: "/img/addplayer.png",
        playerstatus: ['观众', "座位上", "游戏中"],
        roles: ["N/A", '庄', "小盲", "大盲"],
        game: {},
        notification: "请选择座位!",
        preBet: 0,
        showResult: false,
        requested: false
    },
    computed: {
        playerSeats: function () {
            let that = this;
            var onSitePlayes = this.players.filter(function (item) {
                return item.playerStatus >= 1;
            });
            that.seats = new Array(9);
            onSitePlayes.forEach(function (player, index) {
                that.seats.splice(player.index, 1, player);
            })
            return that.seats;
        },
        riverpokers: function () {
            let that = this;
            if (that.game.riverPokers)
                return that.game.riverPokers;
            return [];
        },
        betmoney: function () {
            var money = 0;
            if (this.self.betChips) {
                this.self.betChips.forEach(function (item, index) {
                    money += item.money * item.num;
                });
            }
            return money;
        },
        money: function () {
            var money = 0;
            if (this.self.chips) {
                this.self.chips.forEach(function (item, index) {
                    money += item.money * item.num;
                });
            }
            return money;
        }

    },
    created: function () {
        let that = this;
        $.get(
            "/api/Room",
            { name: "11" },
            function (room) {
                that.$nextTick(() => {
                    that.players = room[0].players;
                    if (room[0].currentGame)
                        that.game = room[0].currentGame;
                    var name = that.$refs.selfName.innerHTML;
                    that.self = that.players.find(function (item, index) {
                        return item.name === name;
                    });
                })
            },
            "json"
        );



        connection.on('AddPlayer', function (data) {
            if (data) {
                that.$nextTick(() => {
                    that.players.push(data);

                })
            }
        });

        connection.on('UpdatePlayer', function (player) {
            if (player) {
                that.$nextTick(() => {
                    var index = that.players.findIndex(function (item) {
                        return item.name === player.name;
                    });

                    that.players.splice(index, 1, player);

                    that.self = that.players.find(function (item, index) {
                        return item.name === that.self.name;
                    });

                    this.requested = false;

                })
            }
        });

        connection.on('UpdatePlayers', function (players) {
            if (players) {
                that.$nextTick(() => {
                    console.log(players);
                    players.forEach(function (player, index) {
                        var index = that.players.findIndex(function (item) {
                            return item.name === player.name;
                        });
                        that.players.splice(index, 1, player);
                    })

                    that.self = that.players.find(function (item, index) {
                        return item.name === that.self.name;
                    });
                    this.requested = false;

                })
            }
        });

        connection.on('UpdateGame', function (game) {
            if (game) {
                that.$nextTick(() => {
                    that.game = game;
                    that.notification = "Game" + that.game.id + "  " + that.game.gameStatus;

                    this.requested = false;

                    if (that.game.isFinished)
                        that.showResult = true;
                })
            }
        });

    },
    methods: {
        startGame: function () {
            connection.invoke('StartGame');
        },
        bet: function () {
           
            //if (this.requested)
            //    return;

            var money = this.self.betMoney;

            if (this.self.role == 2) {
                if (money != 5 && this.game.maxBet == 0) {
                    alert("小盲的第一轮下注必须是5!");
                    return;
                }
            }
            if (this.self.role == 3) {
                if (money != 10 && this.game.maxBet == 5) {
                    alert("大盲的第一轮下注必须是10!");
                    return;
                }
            }
            if (money < this.game.maxBet) {
                alert("你的下注不能少于之前玩家!");
                return;
            }

            if (money == this.preBet) {
                this.check();
            }

            this.preBet = money;
            this.self.isActive = false;
            connection.invoke('Bet', money);
            this.requested = true;
        },
        add: function (bet) {
            if (!this.self.isActive)
                return;
            if (this.self.betMoney + bet <= this.self.money)
                this.self.betMoney += bet;
        },
        remove: function (bet) {
            if (!this.self.isActive)
                return;
            if (this.self.betMoney >= bet)
                this.self.betMoney -= bet;

        },

        check: function () {
           
            //if (this.requested)
            //    return;

            var money = this.self.betMoney;

            if (money < this.game.maxBet) {
                alert("有人下注，当前不能Check!");
                return;
            }

            if (this.self.role == 3 || this.self.role == 2) {
                if (this.preBet == 0) {
                    alert("当前不能Check!");
                    return;
                }
            }

            if (this.preBet != money) {
                alert("您已经押注，不能Check!");
                return;
            }
            this.self.isActive = false;
            connection.invoke('Check');
            this.requested = true;
        },
        fold: function () {
           
            //if (this.requested)
            //    return;

            if (this.self.role == 3 || this.self.role == 2) {
                var money = this.preBet;

                if (money == 0) {
                    alert("当前不能Fold!");
                    return;
                }
            }
            this.self.isActive = false;
            connection.invoke('Fold');
            this.requested = true;
        }

    }
})










