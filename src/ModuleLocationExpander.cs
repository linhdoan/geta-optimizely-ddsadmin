using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace Geta.DdsAdmin
{
    public class ModuleLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            //{2} is area, {1} is controller,{0} is the action
            var locations = new[] { "/Geta.DdsAdmin/Views/{0}.cshtml" };
            return locations.Union(viewLocations);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }
    }
}
