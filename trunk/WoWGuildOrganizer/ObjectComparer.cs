using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;



namespace WoWGuildOrganizer
{
    [Serializable]
    public class ObjectComparer : IComparer
    {
        #region Constructor
        public ObjectComparer()
        {
        }

        public ObjectComparer(string p_propertyName)
        {    
            //We must have a property name for this comparer to work
            this.PropertyName = p_propertyName;
        }

        public ObjectComparer(string p_propertyName, bool p_MultiColumn)
        {
            //We must have a property name for this comparer to work
            this.PropertyName = p_propertyName;
            this.MultiColumn = p_MultiColumn;
        }
        #endregion

        #region Property
        private bool _MultiColumn;
        public bool MultiColumn
        {
            get { return _MultiColumn; }
            set { _MultiColumn = value; }
        }

        private string _propertyName;
        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }
        #endregion 

        #region IComparer<ComparableObject> Members
        /// <summary>
        /// This comparer is used to sort the generic comparer
        /// The constructor sets the PropertyName that is used
        /// by reflection to access that property in the object to 
        /// object compare.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(Object x, Object y)
        {
            Type t = x.GetType();

            if (_MultiColumn) // Multi Column Sorting
            {
                string[] sortExpressions = _propertyName.Trim().Split(',');
                for (int i = 0; i < sortExpressions.Length; i++)
                {
                    string fieldName, direction = "ASC";
                    if (sortExpressions[i].Trim().EndsWith(" DESC"))
                    {
                        fieldName = sortExpressions[i].Replace(" DESC", "").Trim();
                        direction = "DESC";
                    }
                    else
                    {
                        fieldName = sortExpressions[i].Replace(" ASC", "").Trim();
                    }
                    
                    //Get property by name
                    PropertyInfo val = t.GetProperty(fieldName);
                    if (val != null)
                    {
                        //Compare values, using IComparable interface of the property's type
                        int iResult = Comparer.DefaultInvariant.Compare(val.GetValue(x, null), val.GetValue(y, null));
                        if (iResult != 0)
                        {
                            //Return if not equal
                            if (direction == "DESC")
                            {
                                //Invert order
                                return -iResult;
                            }
                            else
                            {
                                return iResult;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception(fieldName + " is not a valid property to sort on. It doesn't exist in the Class.");
                    }
                }
                
                //Objects have the same sort order
                return 0;
            }
            else
            {
                string fieldName, direction = "ASC";
                string sortExpressions = _propertyName.Trim();

                if (sortExpressions.EndsWith(" DESC"))
                {
                    fieldName = sortExpressions.Replace(" DESC", "").Trim();
                    direction = "DESC";
                }
                else
                {
                    fieldName = sortExpressions.Replace(" ASC", "").Trim();
                }

                PropertyInfo val = t.GetProperty(fieldName);
                if (val != null)
                {
                    int iResult = Comparer.DefaultInvariant.Compare(val.GetValue(x, null), val.GetValue(y, null));
                    if (iResult != 0)
                    {
                        //Return if not equal
                        if (direction == "DESC")
                        {
                            //Invert order
                            return -iResult;
                        }
                        else
                        {
                            return iResult;
                        }
                    }
                }
                else
                {
                    throw new Exception(this.PropertyName + " is not a valid property to sort on. It doesn't exist in the Class.");
                }

                //Objects have the same sort order
                return 0;
            }
        }
        #endregion
    }
}
