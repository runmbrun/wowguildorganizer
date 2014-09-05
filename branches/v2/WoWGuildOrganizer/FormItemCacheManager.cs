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
        /// <summary>
        /// 
        /// </summary>
        public FormItemCacheManager()
        {
            this.InitializeComponent();

            //bindingSource1.DataSource = WoWGuildOrganizer.FormMain.Items;
            //bindingNavigator1.BindingSource = bindingSource1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeleteCache_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("ItemCache.dat") && 
                    MessageBox.Show("Are you sure you want to permanently delete the item cache?", "Deleting Item Cache", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    // delete it
                    File.Delete("ItemCache.dat");

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
        private void textBoxItemId_TextChanged(object sender, EventArgs e)
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }


        //public DictionaryBindingList<TKey, TValue> ToBindingList<TKey, TValue>(this IDictionary<TKey, TValue> data)
        //{
            //return new DictionaryBindingList<TKey, TValue>(data);
        //}
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

                List<ItemInfo> test = new List<ItemInfo>();

                test = WoWGuildOrganizer.FormMain.Items.GetData().Values.ToList<ItemInfo>();
                bindingSource1.DataSource = test;
                bindingNavigator1.BindingSource = bindingSource1;

                textBoxItemId.DataBindings.Add("Text", bindingSource1, "id");
                textBoxItemName.DataBindings.Add("Text", bindingSource1, "name");
                textBox3.DataBindings.Add("Text", bindingSource1, "itemlevel");
                textBox4.DataBindings.Add("Text", bindingSource1, "tooltip");
            }
            else
            {
                
            }            
        }

    }
}
