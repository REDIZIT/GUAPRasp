using Xamarin.Forms;

namespace App1
{
    public class ListTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SubjectTemplate { get; set; }
        public DataTemplate BreakTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((ListViewItem)item) is Subject ? SubjectTemplate : BreakTemplate;
        }
    }
}
