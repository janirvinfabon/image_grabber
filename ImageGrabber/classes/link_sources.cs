using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGrabber.classes
{
    public class link_sources
    {
        public int No { get; set; }
        [DisplayName("Image Source")] public string ImageSource { get; set; }
        [Browsable(false)] public string ImageExtension { get; set; }
    }
}
