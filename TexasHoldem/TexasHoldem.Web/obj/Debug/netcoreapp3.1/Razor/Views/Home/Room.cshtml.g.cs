#pragma checksum "C:\Projects\Projects\TH\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\Home\Room.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "80f4cda4f027a8f79c75f595076c3327f109921f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Room), @"mvc.1.0.view", @"/Views/Home/Room.cshtml")]
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
#line 1 "C:\Projects\Projects\TH\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\_ViewImports.cshtml"
using TexasHoldem.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Projects\Projects\TH\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\_ViewImports.cshtml"
using TexasHoldem.Web.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"80f4cda4f027a8f79c75f595076c3327f109921f", @"/Views/Home/Room.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"de4e98390cdfca4557a02549cc8fe43d3544cf1a", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Room : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Projects\Projects\TH\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\Home\Room.cshtml"
  
    ViewData["Title"] = "Room";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Room</h1>\r\n<table class=\"table table-striped\">\r\n    <thead>\r\n        <tr>\r\n            <th>房间名</th>\r\n            <th>操作</th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
#nullable restore
#line 15 "C:\Projects\Projects\TH\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\Home\Room.cshtml"
         foreach (var room in ViewData["Rooms"] as IList<Room>)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>");
#nullable restore
#line 18 "C:\Projects\Projects\TH\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\Home\Room.cshtml"
               Write(room.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td><a");
            BeginWriteAttribute("href", " href=\"", 369, "\"", 399, 2);
            WriteAttributeValue("", 376, "/Home/Index?id=", 376, 15, true);
#nullable restore
#line 19 "C:\Projects\Projects\TH\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\Home\Room.cshtml"
WriteAttributeValue("", 391, room.Id, 391, 8, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\'btn btn-success btn-sm\'>Join</a></td>\r\n            </tr>\r\n");
#nullable restore
#line 21 "C:\Projects\Projects\TH\TexasHoldem\TexasHoldem\TexasHoldem.Web\Views\Home\Room.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n\r\n\r\n");
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
