using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace SourceGit.Views
{
    public class ListBoxItemEx : ListBoxItem
    {
        protected override Type StyleKeyOverride => typeof(ListBoxItem);

        public ListBoxItemEx()
        {
            AddHandler(PointerPressedEvent, OnPointerPressedEnsureSelection, RoutingStrategies.Tunnel);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
                return;

            base.OnKeyDown(e);
        }

        private void OnPointerPressedEnsureSelection(object sender, PointerPressedEventArgs e)
        {
            if (e.Handled || !e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                return;

            if (ItemsControl.ItemsControlFromItemContainer(this) is not ListBox listBox)
                return;

            var item = DataContext;
            if (item == null)
                return;

            var ctrl = OperatingSystem.IsMacOS() ? KeyModifiers.Meta : KeyModifiers.Control;
            if (e.KeyModifiers.HasFlag(ctrl) || e.KeyModifiers.HasFlag(KeyModifiers.Shift))
                return;

            switch (listBox.SelectionMode)
            {
                case SelectionMode.Single:
                    if (!Equals(listBox.SelectedItem, item))
                        listBox.SelectedItem = item;
                    break;
                case SelectionMode.Multiple:
                    if (listBox.SelectedItems == null)
                        break;

                    if (listBox.SelectedItems.Count != 1 || !listBox.SelectedItems.Contains(item))
                    {
                        listBox.SelectedItems.Clear();
                        listBox.SelectedItems.Add(item);
                    }
                    break;
                case SelectionMode.Toggle:
                    listBox.SelectedItem = Equals(listBox.SelectedItem, item) ? null : item;
                    break;
            }
        }
    }

    public class ListBoxEx : ListBox
    {
        protected override Type StyleKeyOverride => typeof(ListBox);

        protected override Control CreateContainerForItemOverride(object item, int index, object recycleKey)
        {
            return new ListBoxItemEx();
        }

        protected override bool NeedsContainerOverride(object item, int index, out object recycleKey)
        {
            return NeedsContainer<ListBoxItemEx>(item, out recycleKey);
        }

        protected void Select(object item)
        {
            SelectedItem = item;
            ScrollIntoView(item);
            ContainerFromItem(item)?.Focus();
        }
    }
}
