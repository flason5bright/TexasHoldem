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
        playerStatus: Number,
        self: Object,
        game: Object
    },
    watch: {
        playerStatus: function () {
            if (this.playerStatus === 2) {
                this.canSelectSeat = false;
            }
            else {
                this.canSelectSeat = true;
            }
        }
    },
    methods: {
        selectSeat: function (index) {
            //if (!this.canSelectSeat) {
            //    alert("游戏中不能换位置!");
            //}
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
        betChips: [],
        preBet:0
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
                })
            }
        });

        connection.on('UpdateGame', function (game) {
            if (game) {
                that.$nextTick(() => {
                    that.game = game;
                    that.notification = "Game" + that.game.id + that.game.gameStatus;
                })
            }
        });

    },
    methods: {
        prepare: function (event) {
            connection.invoke('Prepare', playername);
        },

        startGame: function () {
            connection.invoke('StartGame');
        },
        chipToggle: function (data) {
            if (data.isAdd)
                this.betChips.push(data.chip);
            else {
                var index = this.betChips.indexOf(data.chip);
                this.betChips.splice(index, 1);
            }
        },
        addBet: function (chip) {
            if (chip.num == 0) {
                alert("该筹码已经用完!");
                return;
            }

            let that = this;

            var index = that.self.chips.findIndex(function (item) {
                return item.money == chip.money;
            });
            chip.num--;
            that.self.chips.splice(index, 1, chip);

            var chipIndex = -1;
            var betChip = that.self.betChips.find(function (item, index) {
                if (item.money == chip.money)
                    chipIndex = index;
                return item.money == chip.money;
            });
            betChip.num++;
            that.self.betChips.splice(chipIndex, 1, betChip);
        },
        removeBet: function (betChip) {

            if (betChip.num == 0) {
                alert("该筹码已经退完!");
                return;
            }

            let that = this;

            var betIndex = that.self.betChips.findIndex(function (item) {
                return item.money == betChip.money;
            });
            betChip.num--;
            that.self.betChips.splice(betIndex, 1, betChip);

            var chipIndex = -1;
            var chip = that.self.chips.find(function (item, index) {
                if (item.money == betChip.money)
                    chipIndex = index;
                return item.money == betChip.money;
            });
            chip.num++;
            that.self.chips.splice(chipIndex, 1, chip);
        },
        bet: function () {
            var money = 0;
            this.self.betChips.forEach(function (item, index) {
                money += item.money * item.num;
            });

            if (this.self.role == 2) {
                if (money != 5 && this.game.maxBet == 0) {
                    alert("小盲的第一轮下注必须是5!");
                    return;
                }
            }
            if (this.self.role == 3) {
                if (money != 25 && this.game.maxBet == 0) {
                    alert("大盲的第一轮下注必须是25!");
                    return;
                }
            }
            if (money < this.game.maxBet) {
                alert("你的下注不能少于之前玩家!");
                return;
            }

            if (money == this.preBet) {
                connection.invoke('Check');
            }

            this.preBet = money;

            connection.invoke('Bet', JSON.stringify(this.self.betChips), JSON.stringify(this.self.chips));
        },

        check: function () {
            var money = 0;
            this.self.betChips.forEach(function (item, index) {
                money += item.money * item.num;
            });

            if (money < this.game.maxBet) {
                alert("有人下注，当前不能Check!");
                return;
            }

            if (this.self.role == 3 || this.self.role == 2) {
                if (money == 0) {
                    alert("当前不能Check!");
                    return;
                }
            }

            if (this.preBet != money) {
                alert("您已经押注，不能Check!");
                return;
            }

            connection.invoke('Check');
        },
        fold: function () {
            if (this.self.role == 3 || this.self.role == 2) {
                var money = 0;
                this.self.betChips.forEach(function (item, index) {
                    money += item.money * item.num;
                });

                if (money == 0) {
                    alert("当前不能Fold!");
                    return;
                }
            }
            connection.invoke('Fold');
        }

    }
})










