using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using DemoApp.Properties;

namespace DemoApp.Model
{
    /// <summary>
    /// Represents a child of group. This class
    /// has built-in validation logic. It is wrapped
    /// by the AllChildsViewModel class, which enables it to
    /// be easily displayed and edited by a WPF user interface.
    /// </summary>
    public class Child : IDataErrorInfo
    {
        #region Creation

        public static Child CreateNewChild()
        {
            return new Child();
        }

        public static Child CreateChild(
            int code,
            string name,
            string simpleName,
            int groupCode,
            string imgPath
            )
        {
            return new Child
            {
                Code = code,
                Name = name,
                SimpleName = simpleName,
                GroupCode = groupCode,
                ImgPath = imgPath
            };
        }

        protected Child()
        {
        }

        #endregion // Creation

        #region State Properties

        /// <summary>
        /// Gets/sets the unique code for the child.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets/sets the name for the child.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets the simpleName for the child.
        /// </summary>
        public string SimpleName { get; set; }

        /// <summary>
        /// Gets/sets the GroupCode to which the child belongs.
        /// </summary>
        public int GroupCode { get; set; }

        /// <summary>
        /// Gets/sets the child container background image's path.  
        /// </summary>
        public string ImgPath { get; set; }

        #endregion // State Properties

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error { get { return null; } }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return this.GetValidationError(propertyName); }
        }

        #endregion // IDataErrorInfo Members

        #region Validation

        /// <summary>
        /// Returns true if this object has no validation errors.
        /// </summary>
        public bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                    if (GetValidationError(property) != null)
                        return false;

                return true;
            }
        }

        static readonly string[] ValidatedProperties =
        {
            "Code",
            "Name",
            "SimpleName",
            "GroupCode",
            "ImgPath"
        };

        string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                case "Code":
                    error = this.ValidateCode();
                    break;

                case "Name":
                    error = this.ValidateName();
                    break;

                case "SimpleName":
                    error = this.ValidateSimpleName();
                    break;

                case "GroupCode":
                    error = this.ValidateGroupCode();
                    break;

                case "ImgPath":
                    error = this.ValidateImgPath();
                    break;

                default:
                    Debug.Fail("Unexpected property being validated on Child: " + propertyName);
                    break;
            }

            return error;
        }

        string ValidateCode()
        {
            /*
            if (IsStringMissing(this.Email))
            {
                return Strings.Customer_Error_MissingEmail;
            }
            else if (!IsValidEmailAddress(this.Email))
            {
                return Strings.Customer_Error_InvalidEmail;
            }
            */
            return null;
        }

        string ValidateName()
        {
            /*
            if (IsStringMissing(this.FirstName))
            {
                return Strings.Customer_Error_MissingFirstName;
            }
            */
            return null;
        }

        string ValidateSimpleName()
        {
            /*
            if (IsStringMissing(this.FirstName))
            {
                return Strings.Customer_Error_MissingFirstName;
            }
            */
            return null;
        }

        string ValidateGroupCode()
        {
            /*
            if (IsStringMissing(this.Email))
            {
                return Strings.Customer_Error_MissingEmail;
            }
            else if (!IsValidEmailAddress(this.Email))
            {
                return Strings.Customer_Error_InvalidEmail;
            }
            */
            return null;
        }

        string ValidateImgPath()
        {
            /*
            if (this.IsCompany)
            {
                if (!IsStringMissing(this.LastName))
                    return Strings.Customer_Error_CompanyHasNoLastName;
            }
            else
            {
                if (IsStringMissing(this.LastName))
                    return Strings.Customer_Error_MissingLastName;
            }
            */
            return null;
        }

        /*
        static bool IsStringMissing(string value)
        {
            return
                String.IsNullOrEmpty(value) ||
                value.Trim() == String.Empty;
        }

        static bool IsValidEmailAddress(string email)
        {
            if (IsStringMissing(email))
                return false;

            // This regex pattern came from: http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
        */
        #endregion // Validation

    }
}
