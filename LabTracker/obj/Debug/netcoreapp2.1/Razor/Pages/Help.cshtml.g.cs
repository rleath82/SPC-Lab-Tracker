#pragma checksum "C:\Users\Robert Leatherman\source\repos\LabTracker\LabTracker\Pages\Help.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b807d3a6f6780a7b45eaa5a333c1e360b9b8aa13"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(LabTracker.Pages.Pages_Help), @"mvc.1.0.razor-page", @"/Pages/Help.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/Help.cshtml", typeof(LabTracker.Pages.Pages_Help), null)]
namespace LabTracker.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\Robert Leatherman\source\repos\LabTracker\LabTracker\Pages\_ViewImports.cshtml"
using LabTracker;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b807d3a6f6780a7b45eaa5a333c1e360b9b8aa13", @"/Pages/Help.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7fff0e56b83e50448b9b258f2eeebbfc434351f0", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Help : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\Robert Leatherman\source\repos\LabTracker\LabTracker\Pages\Help.cshtml"
  
    ViewData["Title"] = "Help";

#line default
#line hidden
            BeginContext(65, 2405, true);
            WriteLiteral(@"<h2>Lab Student Tracker Help</h2>
<br />
<p>The Lab Student Tracker is a program that instructors can use to schedule labs that students will sign in and out of. This program will track the times of
    these students and document the information for later use. An instructor can add/remove students, courses and labs. Below are the different navigation
    tools used in this program explained in further detail.</p><br />
<h3>Instructor Page</h3>
<p>The Instructor page (Instructor on the navigation bar) is your personal dashboard to edit your profile, assign courses to your profile, add/create students,
    schedule labs, show your details and delete your profile. Click an icon to perform these tasks.</p><br />
<h3>Students Page</h3>
<p>The Students page (Students on the navigation bar) lists all of the students currently in the program. 
    From this page you can edit students, show student details (courses and labs), delete students, 
    and assign courses and labs to students. You can also add/c");
            WriteLiteral(@"reate new students from this page.</p><br />
<h3>Courses Page</h3>
<p>The Courses page (Courses on the navigation bar) lists all of the courses currently assigned to you. Here you can edit, delete, show course details, 
    and enroll students in courses. If you need to assign additional courses to your profile, please navigate to your Instructor page to do so.</p><br />
<h3>Labs Page</h3>
<p>The Labs page (Labs on the navigation bar) lists all of the labs that you have scheduled. You can edit, delete, enroll students, show lab details, and
    launch a lab from this page. When you launch a lab, a pop-up screen will occur for students to sign in and out of the lab. (NOTE: once you launch a lab,
    you cannot navigate back to your profile. You must click the ""End Lab"" button, which will sign you out of the lab and send you to the login page where
    you can sign back in to your profile.)</p><br />
<h3>Dashboard Page</h3>
<p>The Dashboard page (Dashboard on the navigation bar) lists all of the instr");
            WriteLiteral(@"uctors, their assigned courses with students enrolled in thoses courses, 
    and the labs scheduled by those instructors, with the students enrolled in those labs. This is a ""read-only"" page and you cannot edit any information.
    If you need to edit a lab, course, or student enrollment, please navigate back to your Instructor page to do so.</p>


");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<HelpModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<HelpModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<HelpModel>)PageContext?.ViewData;
        public HelpModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591