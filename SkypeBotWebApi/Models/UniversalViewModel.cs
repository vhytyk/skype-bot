using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using SkypeBotRulesLibrary.Entities;

namespace SkypeBotWebApi.Models
{
    public class UniversalViewModel
    {
        public UniversalViewModel(Type type) { ItemType = type; }
        public Type ItemType { get; set; }
        public List<object> All { get; set; }
        public List<PropertyDescriptor> AllProperties {
            get
            {
                return TypeDescriptor.GetProperties(ItemType).Cast<PropertyDescriptor>().ToList();
            }
        }

        public int GetIdValue(object item)
        {
            return (int)TypeDescriptor.GetProperties(ItemType)["Id"].GetValue(item);
        }
    }
}