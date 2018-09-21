using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text.RegularExpressions;
using DemoApp.Properties;

namespace DemoApp.Model
{
    /// <summary>
    /// Represents a group of products.  This class
    /// has built-in validation logic. It is wrapped
    /// by the AllGroupsViewModel class, which enables it to
    /// be easily displayed and edited by a WPF user interface.
    /// </summary>
    [Table("Groups")]
    public class Group : IDataErrorInfo
    {
        #region Creation

        public static Group CreateNewGroup()
        {
            return new Group();
        }

        public static Group CreateGroup(
            int id,
            string name,
            string imgPath
            )
        {
            return new Group
            {
                Id = id,
                Name = name,
                ImgPath = imgPath
            };
        }

        protected Group()
        {
        }

        #endregion // Creation

        #region State Properties

        /// <summary>
        /// Gets/sets the unique id for the group.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets/sets the name for the group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets the group background image's path.  
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
            "Id",
            "Name",
            "ImgPath"
        };

        string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                case "Id":
                    error = this.ValidateId();
                    break;

                case "Name":
                    error = this.ValidateName();
                    break;

                case "ImgPath":
                    error = this.ValidateImgPath();
                    break;

                default:
                    Debug.Fail("Unexpected property being validated on Group: " + propertyName);
                    break;
            }

            return error;
        }

        string ValidateId()
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
