using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Windows.Forms;
using Microsoft.Practices.ObjectBuilder2;

namespace SkypeBot
{
    public partial class WindowTree : Form
    {
        private readonly AutomationElement _element;

        public WindowTree(AutomationElement element = null)
        {
            _element = element;
            InitializeComponent();
            TreeNode childNode = CreateTreeNode(element);
            treeView1.Nodes.Add(childNode);
            CreateNodes(childNode, element);
        }

        TreeNode CreateTreeNode(AutomationElement element)
        {
            return new TreeNode(string.IsNullOrEmpty(element.Current.ClassName) ? element.Current.Name : element.Current.ClassName);
        }
        void CreateNodes(TreeNode parentNode, AutomationElement parentElement)
        {
            parentElement.FindAll(TreeScope.Children, Condition.TrueCondition).Cast<AutomationElement>().ForEach(
                childElement =>
                {
                    TreeNode childNode = CreateTreeNode(childElement);
                    parentNode.Nodes.Add(childNode);
                    CreateNodes(childNode, childElement);
                });
        }
    }
}
