using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetMediatheque.Models
{
    public enum MenuItemType
    {
        Photos,
        Video,
        Fichier
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
