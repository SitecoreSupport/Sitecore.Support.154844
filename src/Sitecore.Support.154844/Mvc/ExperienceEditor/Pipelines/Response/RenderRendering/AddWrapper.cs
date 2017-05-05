using System;
using Sitecore.Diagnostics;
using Sitecore.Mvc.ExperienceEditor.Presentation;

namespace Sitecore.Support.Mvc.ExperienceEditor.Pipelines.Response.RenderRendering
{
    public class AddWrapper : Sitecore.Mvc.ExperienceEditor.Pipelines.Response.RenderRendering.AddWrapper
    {
        public override void Process(Sitecore.Mvc.Pipelines.Response.RenderRendering.RenderRenderingArgs args)
        {
            if(!string.IsNullOrEmpty(args.Rendering.DataSource) && args.PageContext.Item != null && args.PageContext.Item.Database.Name != "core" && args.PageContext.Item.Database.GetItem(args.Rendering.DataSource) == null)
            {
                Log.Warn(string.Format("'{0}' is not valid datasource.", args.Rendering.DataSource), this);
                args.AbortPipeline();
                return;
            }
            if (args.Rendered)
            {
                return;
            }
            if (Context.Site == null)
            {
                return;
            }
            if (!Context.PageMode.IsExperienceEditorEditing)
            {
                return;
            }
            IMarker marker = this.GetMarker();
            if (marker == null)
            {
                return;
            }
            int num = args.Disposables.FindIndex((IDisposable x) => x.GetType() == typeof(Wrapper));
            if (num < 0)
            {
                num = 0;
            }
            args.Disposables.Insert(num, new Wrapper(args.Writer, marker));
        }
    }
}