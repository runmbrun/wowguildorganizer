using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WoWGuildOrganizer
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FormItemCacheManager : Form
    {
        string ItemCacheFile = "ItemCache.dat";

        /// <summary>
        /// 
        /// </summary>
        public FormItemCacheManager()
        {
            this.InitializeComponent();

            //bindingSource1.DataSource = WoWGuildOrganizer.FormMain.Items;
            //bindingNavigator1.BindingSource = bindingSource1;
        }

        public FormItemCacheManager(string itemCacheFile)
        {
            this.InitializeComponent();

            //bindingSource1.DataSource = WoWGuildOrganizer.FormMain.Items;
            //bindingNavigator1.BindingSource = bindingSource1;

            ItemCacheFile = itemCacheFile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDeleteCache_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(ItemCacheFile) && 
                    MessageBox.Show("Are you sure you want to permanently delete the item cache?", "Deleting Item Cache", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    // delete it
                    File.Delete(ItemCacheFile);

                    // clear the current cache
                    WoWGuildOrganizer.FormMain.Items = new ItemCache();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxItemId_TextChanged(object sender, EventArgs e)
        {
            // todo: remove this!
            TextBox t = null; // sender as TextBox;

            if (t != null)
            {
                //say you want to do a search when user types 3 or more chars
                if (t.Text.Length >= 3)
                {
                    //SuggestStrings will have the logic to return array of strings either from cache/db
                    string[] arr = SuggestStrings(t.Text);

                    AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
                    if (arr != null)
                    {
                        collection.AddRange(arr);
                    }

                    this.textBoxItemId.AutoCompleteCustomSource = collection;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string[] SuggestStrings(string text)
        {
            string[] results = null;
            int i = (int)Convert.ToInt32(text);

            //WoWGuildOrganizer.FormMain.Items.Contains(Convert.ToInt32(text));
            //foreach (Item i in WoWGuildOrganizer.FormMain.Items)
            {
                
            }

            /*results = WoWGuildOrganizer.FormMain.Items.Keys.ToList<int>().FindAll(
                delegate(int bk)
                {
                    return WoWGuildOrganizer.FormMain.Items.Keys.Contains(i);
                }
                );*/

            //ItemCache item = new ItemCache();
            //WoWGuildOrganizer.FormMain.Items.Search(i);

            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        public sealed class Pair<TKey, TValue>
        {
            private readonly TKey key;
            private readonly IDictionary<TKey, TValue> data;

            public Pair(TKey key, IDictionary<TKey, TValue> data)
            {
                this.key = key;
                this.data = data;
            }

            public TKey Key { get { return key; } }

            public TValue Value
            {
                get
                {
                    TValue value;
                    data.TryGetValue(key, out value);
                    return value;
                }
                set { data[key] = value; }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        public class DictionaryBindingList<TKey, TValue> : BindingList<Pair<TKey, TValue>>
        {
            private readonly IDictionary<TKey, TValue> data;

            public DictionaryBindingList(IDictionary<TKey, TValue> data)
            {
                this.data = data;
                Reset();
            }

            public void Reset()
            {
                bool oldRaise = RaiseListChangedEvents;
                RaiseListChangedEvents = false;

                try
                {
                    Clear();
                    foreach (TKey key in data.Keys)
                    {
                        Add(new Pair<TKey, TValue>(key, data));
                    }
                }
                finally
                {
                    RaiseListChangedEvents = oldRaise;
                    ResetBindings();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (bindingSource1.DataSource == null)
            {
                /*
                Dictionary<Int32, ItemInfo> test = new Dictionary<int,ItemInfo>();

                test = WoWGuildOrganizer.FormMain.Items.GetData();
                bindingSource1.DataSource = new DictionaryBindingList<Int32, ItemInfo>(test);
                bindingNavigator1.BindingSource = bindingSource1;

                textBoxItemId.DataBindings.Add("Text", bindingSource1, "Key");
                textBox3.DataBindings.Add("Text", bindingSource1, "Value");
                 * */

                DataTable dt = WoWGuildOrganizer.FormMain.Items.GetData();

                bindingSource1.DataSource = dt;
                bindingNavigator1.BindingSource = bindingSource1;

                textBoxItemId.DataBindings.Add("Text", bindingSource1, "id");
                textBoxItemName.DataBindings.Add("Text", bindingSource1, "name");
                textBox3.DataBindings.Add("Text", bindingSource1, "context");
                textBox4.DataBindings.Add("Text", bindingSource1, "iteminfo");
            }
            else
            {
                
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFindItem_Click(object sender, EventArgs e)
        {
            try
            {
                int index = bindingSource1.Find("id", this.textBox1FindItem.Text);

                if (index >= 0)
                {
                    bindingSource1.Position = index;
                }
                else
                {
                    MessageBox.Show("Item not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Item not found. [" + ex.Message + "]");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private DataTable ConvertToDatatable<T>(List<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];

                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                }
                else
                {
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }
            }

            object[] values = new object[props.Count];

            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }

                table.Rows.Add(values);
            }

            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRegetItem_Click(object sender, EventArgs e)
        {
            // reget the item
            
            // First fetch the data
            GetItemInfo get = new GetItemInfo();
            int itemId = Convert.ToInt32(textBoxItemId.Text);
            string itemContext = textBoxItemName.Text;

            if (get.CollectData(itemId, itemContext))
            {
                ItemInfo item = new ItemInfo();

                item = get.Item;

                MessageBox.Show(string.Format("{0} - {1}\n\n{2}", item.Id.ToString(), item.Name, item.Tooltip));
            }
            else
            {
                MessageBox.Show("Item not found. [" + itemId.ToString() + "]");
            }
        }
    }
}
