#pragma checksum "F:\DotNetProjects\TexasHoldem\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "03364b0764af26eda729e55667772a5c5aa3c383"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "F:\DotNetProjects\TexasHoldem\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\_ViewImports.cshtml"
using TexasHoldem.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\DotNetProjects\TexasHoldem\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\_ViewImports.cshtml"
using TexasHoldem.Web.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"03364b0764af26eda729e55667772a5c5aa3c383", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"de4e98390cdfca4557a02549cc8fe43d3544cf1a", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("text/javascript"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/signalr/dist/browser/signalr.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "~/js/site.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "F:\DotNetProjects\TexasHoldem\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<style>
    .img {
        width: 50px;
        height: 50px;
        margin-right: 10px;
        border-radius: 10px;
    }

    #selfImg {
        width: 80px;
        height: 80px;
        margin-right: 10px;
        border-radius: 10px;
    }

    .desktop {
        width: 100%;
        height: 400px;
        background-image: url(/img/desktop4.png);
        background-size: 100% 100%;
        background-position: center;
        display: flex;
        align-items: center;
        justify-content: center;
        flex-direction: column;
    }

    .flex-center {
        display: flex;
        align-items: center;
        justify-content: center;
    }


    .flex-top {
        display: flex;
        align-items: flex-start;
        justify-content: center;
    }

    .desktop .riverpoker {
        width: 55px;
        height: 60px;
        margin: 10px;
    }


    .game {
        display: -webkit-flex; /* Safari */
        display: flex;
        flex-dire");
            WriteLiteral(@"ction: row;
        justify-content: space-around;
        align-items: stretch;
        background-color: #202020;
    }

    .hearderRow1 {
        display: flex;
        align-items: flex-end;
        justify-content: space-around;
        background-color: #202020;
        border: solid 0px white;
    }

    .hearderRow2 {
        display: flex;
        align-items: flex-start;
        justify-content: space-around;
        background-color: #202020;
        border: solid 0px white;
    }

    .headercolumn {
        display: flex;
        flex-direction: column;
        justify-content: space-around;
        background-color: #202020;
        border: solid 0px white;
    }

    .gameheader {
        width: 60px;
        height: 60px;
        margin-top: 5px;
        margin-bottom: 5px;
        border-radius: 10px;
        background-color: #202020;
    }

    #selfPanel {
        display: flex;
        align-items: center;
        justify-content: center;
    }");
            WriteLiteral(@"

    .chip {
        width: 30px;
        height: 30px;
        margin: 3px;
        border-radius: 15px;
    }


    .chip-little {
        width: 10px;
        height: 10px;
        margin: 1px;
        border-radius: 5px;
    }

    .chips {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        align-items: flex-start;
        justify-content: flex-start;
        max-width: 150px;
        margin-right: 10px;
    }

        .chips div input[type='checkbox'] {
            opacity: 0;
            position: absolute;
            left: 0;
            bottom: 0;
            top: 0;
            right: 0;
            height: 10px;
            width: 10px;
        }

        .chips div label:hover img {
            -webkit-transform: scale(1.05);
            -moz-transform: scale(1.05);
            -ms-transform: scale(1.05);
            -o-transform: scale(1.05);
            transform: scale(1.05);
            border: 4px solid #1E9FFF;
  ");
            WriteLiteral(@"      }

        .chips div input[type='checkbox']:checked ~ label img {
            border: 4px solid #1E9FFF;
        }

    .allchips {
        display: flex;
        flex-direction: row;
        align-items: flex-start;
        justify-content: space-around;
        width: 100%;
    }

    .riches {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        align-items: center;
        justify-content: center;
        margin-right: 10px;
    }

    .gamer {
        width: 120px;
        margin: 5px;
    }

        .gamer .header {
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            width: 100%;
            border: solid 0px white;
        }

    .selfpoker {
        width: 25px;
        height: 30px;
        margin: 2px;
        display: inline-block;
    }

    .active {
        border: solid 2px yellow;
    }

    .playerbet {
        border: solid");
            WriteLiteral(" 2px white;\r\n        background: #999999;\r\n        opacity: 0.7;\r\n        margin: 2px;\r\n        color: white;\r\n    }\r\n</style>\r\n\r\n<div class=\"text-center\">\r\n");
            WriteLiteral(@"
    <div id=""app"" class=""container"">

        <div>
            <div class=""hearderRow1"">
                <gamer :seat=""playerSeats[0]"" :self=""self"" :player-status=""self.playerStatus"" :index=""0""></gamer>
                <gamer :seat=""playerSeats[8]"" :self=""self"" :player-status=""self.playerStatus"" :index=""8""></gamer>
            </div>
            <div class=""game"">
                <div class=""headercolumn"">
                    <gamer :seat=""playerSeats[1]"" :self=""self"" :player-status=""self.playerStatus"" :index=""1""></gamer>
                    <gamer :seat=""playerSeats[2]"" :self=""self"" :player-status=""self.playerStatus"" :index=""2""></gamer>
                </div>
");
            WriteLiteral(@"                <div id=""desktop"" class=""desktop"">
                    <div class=""flex-top"">
                        <div v-for=""player in playerSeats"">
                            <div v-if=""player && player.betMoney>0"" class=""playerbet flex-top"">
                                <div>
                                    <span>{{player.name}}</span><br />
                                    <span>{{player.betMoney}}</span><br />
                                </div>
                                <div>
                                    <div v-for=""betChip in player.betChips"">
                                        <div v-if=""betChip.num>0"">
                                            <img class=""chip-little"" :src=""'/img/chip'+betChip.money+'.png'"" />x{{betChip.num}}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
 ");
            WriteLiteral(@"                   <div class=""flex-center"">
                        <div v-for=""poker in riverpokers"">
                            <img v-if=""poker"" class=""riverpoker"" :src=""poker.img"" />
                        </div>
                    </div>
                    <div>
                        <game-pool v-if=""game"" :game=""game""></game-pool>
                    </div>
                </div>
                <div class=""headercolumn"">
                    <gamer :seat=""playerSeats[6]"" :self=""self"" :player-status=""self.playerStatus"" :index=""6""></gamer>
                    <gamer :seat=""playerSeats[7]"" :self=""self"" :player-status=""self.playerStatus"" :index=""7""></gamer>
                </div>
            </div>
            <div class=""hearderRow2"">
                <gamer :seat=""playerSeats[3]"" :self=""self"" :player-status=""self.playerStatus"" :index=""3""></gamer>
                <gamer :seat=""playerSeats[4]"" :self=""self"" :player-status=""self.playerStatus"" :index=""4""></gamer>
                <gamer :");
            WriteLiteral("seat=\"playerSeats[5]\" :self=\"self\" :player-status=\"self.playerStatus\" :index=\"5\"></gamer>\r\n            </div>\r\n        </div>\r\n        <h2 ref=\"selfName\" hidden>");
#nullable restore
#line 253 "F:\DotNetProjects\TexasHoldem\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\Home\Index.cshtml"
                             Write(ViewBag.Player.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n        <h2 ref=\"selfName\" hidden>");
#nullable restore
#line 254 "F:\DotNetProjects\TexasHoldem\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\Home\Index.cshtml"
                             Write(ViewBag.Player.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h2>
        <div>
            <marquee><h2 style=""color:red"">{{notification}}</h2></marquee>
        </div>

        <div>
            <div class=""m-auto"">
                <div id=""selfPanel"">

                    <img id=""selfImg"" :src=""self.avatar"" />
                    <div>
                        <label><b>{{self.name}}</b></label> <br />
                        <label>{{self.money}}</label><br />
                        <label>{{roles[self.role]}}</label>
                    </div>
                    <div class=""allchips"">
                        <div v-for=""(chip, index) in self.chips"">
                            <chip-panel v-bind:chip=""chip"" v-on:chip-toggle=""chipToggle""></chip-panel>
                        </div>
                    </div>
                    <div>
                        <input type=""button"" id=""btnBet"" v-if=""self.isActive"" v-on:click=""bet"" value=""下注"" class=""btn btn-success btn-block"" />
                        <input type=""button"" id=""btnCall"" v-if=""sel");
            WriteLiteral(@"f.isActive"" v-on:click=""bet"" value=""跟注"" class=""btn btn-success btn-block"" />
                        <input type=""button"" id=""btnAllIn"" v-if=""self.isActive"" v-on:click=""bet"" value=""All In"" class=""btn btn-success btn-block"" />
                    </div>
                </div>
            </div>
            <div>
                <input type=""button"" id=""btnStart"" v-if=""self.name=='Tony'"" v-on:click=""startGame"" value=""Start"" class=""btn btn-success btn-block"" />
            </div>

        </div>
        <div>
            <label>玩家列表</label>
            <table class=""table table-striped"">
                <thead>
                    <tr>
                        <th></th>
                        <th>玩家</th>
                        <th>资产</th>
                        <th>状态</th>
                    </tr>
                </thead>
                <tbody>

                    <tr v-for=""(player, index) in players"">
                        <td>{{index + 1}}</td>
                        <td style");
            WriteLiteral(@"=""text-align:left""><img class=""img"" v-bind:src=""player.avatar"" /><span>{{player.name}}</span></td>
                        <td style=""text-align:center"">
                            <riches :player=""player""></riches>
                        </td>
                        <td>{{playerstatus[player.playerStatus]}}</td>
                    </tr>

                </tbody>
            </table>
        </div>
    </div>
</div>

<template id=""riches"">
    <div class=""riches"">
        <label>{{player.money}}:</label>
        <div v-for=""(chip, index) in player.chips"">
            <img class=""chip"" :src=""'/img/chip'+chip.money+'.png'"" />x{{chip.num}}
        </div>
    </div>
</template>
<template id=""gamePool"">
    <div class=""riches"" style=""color:white"">
        <label>{{game.poolMoney}}:</label>
        <div v-for=""(chip, index) in game.poolChips"">
            <img class=""chip"" :src=""'/img/chip'+chip.money+'.png'"" />x{{chip.num}}
        </div>
    </div>
</template>

<template id=""chip");
            WriteLiteral(@"s"">
    <div class=""chips"">
        <div v-for='(item,index) in chip.num'>
            <input :id=""chip.money + index"" type=""checkbox"" v-on:click=""chipToggle"" :value=""chip"" />
            <label :for=""chip.money + index"">
                <img class=""chip"" :src=""'/img/chip'+chip.money+'.png'"" />
            </label>
        </div>
    </div>
</template>

<template id=""gamer"">
    <div class=""gamer"">
        <div v-if=""seat"" :class=""{ active: seat.isActive }"">
            <div class=""header"">
                <img class=""gameheader"" :src=""seat.avatar"" />
                <div style=""margin:5px; float:left"">
                    <span><b>{{seat.name}}</b></span> <br />
                    <span>{{seat.money}}</span><br />
                    <span>{{roles[seat.role]}}</span>
                </div>
            </div>
            <div class=""header"">
                <div style=""margin:2px; display:flex"" v-if=""seat.poker1"">
                    <img class=""selfpoker"" :src=""getFirstImg()"" />
   ");
            WriteLiteral(@"                 <img class=""selfpoker"" :src=""getSecondImg()"" />
                </div>
            </div>
        </div>
        <div v-else class=""header"">
            <img class=""gameheader"" :src=""addplayersrc"" v-on:click=""selectSeat(index)"" />
        </div>
    </div>
</template>


");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "03364b0764af26eda729e55667772a5c5aa3c38317324", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "03364b0764af26eda729e55667772a5c5aa3c38318447", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
#nullable restore
#line 368 "F:\DotNetProjects\TexasHoldem\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\Home\Index.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion = true;

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-append-version", __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
