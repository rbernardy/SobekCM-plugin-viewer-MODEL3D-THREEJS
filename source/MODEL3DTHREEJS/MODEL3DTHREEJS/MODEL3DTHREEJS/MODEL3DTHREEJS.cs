using System;
using System.Web;
using System.IO;

using SobekCM.Core.BriefItem;
using SobekCM.Core.FileSystems;
using SobekCM.Core.Navigation;
using SobekCM.Core.Users;
using SobekCM.Engine_Library;
using SobekCM.Library.ItemViewer.Viewers;
using SobekCM.Library.UI;
using SobekCM.Tools;

namespace MODEL3DTHREEJS
{
    class MODEL3DTHREEJS_ItemViewer : abstractNoPaginationItemViewer
    {
        /// <summary> Constructor for a new instance of the MODEL3DTHREEJS_ItemViewer class, used to display a 3D file from a digital resource </summary>
        /// <param name="BriefItem"> Digital resource object </param>
        /// <param name="CurrentUser"> Current user, who may or may not be logged on </param>
        /// <param name="CurrentRequest"> Information about the current request </param>
        /// <param name="Tracer"> Trace object keeps a list of each method executed and important milestones in rendering </param>
        public MODEL3DTHREEJS_ItemViewer(BriefItemInfo BriefItem, User_Object CurrentUser, Navigation_Object CurrentRequest, Custom_Tracer Tracer)
        {
            this.BriefItem = BriefItem;
            this.CurrentRequest = CurrentRequest;
            this.CurrentUser = CurrentUser;
        }

        public override void Add_Main_Viewer_Section(System.Web.UI.WebControls.PlaceHolder MainPlaceHolder, Custom_Tracer Tracer)
        {
            // do nothing
        }

        /// <summary> Write any additional values within the HTML Head of the final served page </summary>
        /// <param name="Output"> Output stream currently within the HTML body tags </param>
        /// <param name="Tracer"> Trace object keeps a list of each method executed and important milestones in rendering </param>
        public override void Write_Main_Viewer_Section(TextWriter Output, Custom_Tracer Tracer)
        {
            //String baseurl = CurrentRequest.Base_URL + "plugins/MODEL3DTHREEJS/models/";
            String baseurl = "/plugins/MODEL3DTHREEJS/models/";

            Output.WriteLine("<script type=\"text/javascript\">");
            Output.WriteLine("  $('#itemNavForm').prop('action','').submit(function(event){ event.preventDefault(); });");
            Output.WriteLine("</script>");

            // Is there a .obj file in the content folder?  Otherwise, use the models/?.obj sample

            string source_url = "female-croupier-2013-03-26.obj";
            string mtl_url = "female-croupier-2013-03-26.mtl";

            string network_location;

            Output.WriteLine("<!-- bibid=[" + BriefItem.BibID + ":" + BriefItem.VID + "] -->");

            try
            {
                network_location = SobekFileSystem.Resource_Network_Uri(BriefItem);
                Output.WriteLine("<!-- network_location=[" + network_location + "]. -->");
            }
            catch (Exception e)
            {
                Output.WriteLine("<!-- exception trying to get network_location... -->");
                network_location = "C:/inetpub/wwwroot/temp/";
            }
            
            // Does an *.obj file exist?

            String[] files_obj = Directory.GetFiles(network_location, "*.obj", SearchOption.TopDirectoryOnly);
            String[] files_mtl = Directory.GetFiles(network_location, "*.mtl", SearchOption.TopDirectoryOnly);

            if (!(files_obj.Length > 0 && files_mtl.Length > 0))
            {
                Output.WriteLine("<!-- doesn't have both a obj and mtl file -->");
            }
            else
            {
                baseurl = SobekFileSystem.AssociFilePath(BriefItem).Replace("\\", "/");

                if (files_obj.Length > 0)
                {
                    Output.WriteLine("<!-- has an obj file. -->");
                    //baseurl = UI_ApplicationCache_Gateway.Settings.Servers.Image_URL + SobekFileSystem.AssociFilePath(BriefItem).Replace("\\", "/");
                    baseurl = "/content/" + SobekFileSystem.AssociFilePath(BriefItem).Replace("\\", "/");
                    source_url = Path.GetFileName(files_obj[0]);
                    Output.WriteLine("<!-- source_url=[" + source_url + "]. -->");
                }
                else
                {
                    Output.WriteLine("<!-- no obj files found, using default -->");
                }

                if (files_mtl.Length > 0)
                {
                    Output.WriteLine("<!-- has an mtl file. -->");
                    //baseurl = UI_ApplicationCache_Gateway.Settings.Servers.Image_URL + SobekFileSystem.AssociFilePath(BriefItem).Replace("\\", "/");
                    baseurl = "/content/" + SobekFileSystem.AssociFilePath(BriefItem).Replace("\\", "/");
                    mtl_url = Path.GetFileName(files_mtl[0]);
                    Output.WriteLine("<!-- mtl_url=[" + mtl_url + "]. -->");
                }
                else
                {
                    Output.WriteLine("<!-- no mtl files found, using default -->");
                }
            }

            Output.WriteLine("<!-- baseurl=[" + baseurl + "]. -->");

            // Fit into SobekCM item page
            Output.WriteLine("    <td style=\"width:100%; height:100%;\">");

            // write tool container
            Output.WriteLine("        <div id=\"threejsdiv\"></div>");

            // Fit into SobekCM item page 
            Output.WriteLine("    </td>");

            //jquery document ready
            Output.WriteLine("<script type=\"text/javascript\">");
            Output.WriteLine("$(document).ready(function()");
            Output.WriteLine("{");
            Output.WriteLine("\tconsole.log(\"additional jquery doc ready.\");");
            Output.WriteLine("\tconsole.log(\"baseurl=[" + baseurl + "], source_url=[" + source_url + "], mtl_url=[" + mtl_url + "].\");");
            Output.WriteLine("\tsetupTHREEJS('" + baseurl + "','" + source_url + "','" + mtl_url + "');");
            Output.WriteLine("\t$(\".sbkIsw_DocumentDisplay2\").css(\"width\",\"100%\").css(\"height\",\"100%\");");
            Output.WriteLine("});");
            Output.WriteLine("</script>");
        }

        /// <summary> Write any additional values within the HTML Head of the final served page </summary>
        /// <param name="Output"> Output stream currently within the HTML head tags </param>
        /// <param name="Tracer"> Trace object keeps a list of each method executed and important milestones in rendering </param>
        /// <remarks> By default this does nothing, but can be overwritten by all the individual item viewers </remarks>
        public override void Write_Within_HTML_Head(TextWriter Output, Custom_Tracer Tracer)
        {
            String baseurl = CurrentRequest.Base_URL + "plugins/MODEL3DTHREEJS/";

            Output.WriteLine("\t<script type=\"text/javascript\" src=\"" + baseurl + "js/three.js\"></script>");
            //Output.WriteLine("\t<script type=\"text/javascript\" src=\"" + baseurl + "js/three.module.js\"></script>");
            Output.WriteLine("\t<script type=\"text/javascript\" src=\"" + baseurl + "js/Detector.js\"></script>");
            Output.WriteLine("\t<script type=\"text/javascript\" src=\"" + baseurl + "js/OrbitControls.js\"></script>");
            Output.WriteLine("\t<script type=\"text/javascript\" src=\"" + baseurl + "js/OBJLoader.js\"></script>");
            Output.WriteLine("\t<script type=\"text/javascript\" src=\"" + baseurl + "js/MTLLoader.js\"></script>");
            Output.WriteLine("\t<script type=\"text/javascript\" src=\"" + baseurl + "js/setupTHREEJS.js\"></script>");
        }
    }
}