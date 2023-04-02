using System;
using La_Grazia.Database.Models;

namespace La_Grazia.ViewModels
{
    public class AboutViewModel
    {
        public About Abouts { get; set; }
        public Setting Settings { get; set; }
        public List<Feature> Features { get; set; }
        public List<Team> Teams { get; set; }
    }
}