using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGrabber.classes
{
    public class img_types
    {
        public int Id { get; set; }
        public string ImageType { get; set; }
        public string Extension { get; set; }

        public List<img_types> Fetch(List<link_sources> links)
        {
            var counter = 1;
            var data = new List<img_types>();

            data.AddRange(new List<img_types>() { new img_types() { Id = 0, ImageType = "All files", Extension = "all" } });
            links.ForEach((x) => 
            {
                data.AddRange(new List<img_types>() 
                { 
                    new img_types() 
                    { 
                        Id = counter++, 
                        Extension = x.ImageExtension,
                        ImageType = string.Format("{0} files", x.ImageExtension.ToUpper()) 
                    } 
                });
            });

            return data.OrderBy(x => x.Id).ToList();
        }
    }
}
