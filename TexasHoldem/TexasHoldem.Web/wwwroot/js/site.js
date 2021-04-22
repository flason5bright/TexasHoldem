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



Vue.component('chip-panel', {
    data: function () {
        return {
            betChips: []
        }
    },
    props: {
        chip: Object
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
        self: Object
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
            if (this.self.name == this.seat.name)
                return this.seat.poker1.img;
            return this.seat.poker1.backImg;
        },
        getSecondImg: function () {
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
        betChips: []
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
        bet: function () {
            connection.invoke('Bet', JSON.stringify(this.betChips));
        }

    }
})










