using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebApp.Models.Documents {
    public class BaseDocument {
        public BaseDocument() {

        }
        /// <summary>
        /// Do not write this value to the server. Use only for search and retrieve
        /// </summary>
        [BsonIgnoreIfNull]
        public string Id { get; set; }

        /// <summary>
        /// The user who inserted the data
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("user")]
        public string User { get; set; }

        /// <summary>
        /// Type is 'Environment' and/or 'Health'
        /// </summary>
        [BsonRequired]
        [BsonElement("type")]
        [Required]
        public List<string> Datatype { get; set; }

        [BsonIgnore]
        [DisplayName("Environment")]
        public bool DatatypeEnvironment {
            get {
                bool variable = false;
                if (Datatype == null) return variable;
                foreach (string d in Datatype) {
                    if (string.IsNullOrEmpty(d)) {
                        if (d.Contains("Environment")) variable = true;
                    }
                }
                return variable;
            }
            set {
                if (value) {
                    bool variable = false;
                    if (Datatype == null) Datatype = new List<string>();
                    foreach (string d in Datatype) {
                        if (string.IsNullOrEmpty(d)) {
                            if (d.Contains("Environment")) variable = true;
                        }
                    }
                    if (variable) Datatype.Add("Environment");
                }
            }
        }

        [BsonIgnore]
        [DisplayName("Health")]
        public bool DatatypeHealth {
            get {
                bool variable = false;
                if (Datatype == null) return variable;
                foreach (string d in Datatype) {
                    if (string.IsNullOrEmpty(d)) {
                        if (d.Contains("Health")) variable = true;
                    }
                }
                return variable;
            }
            set {
                if (value) {
                    bool variable = false;
                    if (Datatype == null) Datatype = new List<string>();
                    foreach (string d in Datatype) {
                        if (string.IsNullOrEmpty(d)) {
                            if (d.Contains("Health")) variable = true;
                        }
                    }
                    if (variable) Datatype.Add("Health");
                }
            }
        }

        /// <summary>
        /// Title of data
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("name")]
        [DisplayName("Title of document")]
        [Required]
        public string Name { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("author")]
        [DisplayName("Author")]
        [Required]
        public string Author { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("date")]
        [DisplayName("Date of publication")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime? Date { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("dateStored")]
        [DisplayName("Date added")]
        [DataType(DataType.Date)]
        public DateTime DateStored { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("keywords")]
        [Display(Name = "Keyword", Prompt = "Measurement, Emissions, Ships, Sea, Norway")]
        [Required]
        public string Keywords { get; set; }

        [BsonIgnore]
        [BsonElement("public")]
        [DisplayName("Is this public data?")]
        [DataType("Checkmark")]
        [EnforceTrue(ErrorMessage = "You cannot upload data that is not public")]
        [Required]
        public bool Public { get; set; }

        [BsonElement("aggregated")]
        [BsonIgnoreIfNull]
        public AggregatedData Aggregated { get; set; }


    }

    public class EnforceTrueAttribute : ValidationAttribute, IClientValidatable {
        public override bool IsValid(object value) {
            if (value == null) return false;
            if (value.GetType() != typeof(bool)) throw new InvalidOperationException("can only be used on boolean properties.");
            return (bool)value == true;
        }

        public override string FormatErrorMessage(string name) {
            if (string.IsNullOrWhiteSpace(ErrorMessage))
                return "The " + name + " field must be checked in order to continue.";
            else return ErrorMessage;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context) {
            yield return new ModelClientValidationRule {
                ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? FormatErrorMessage(metadata.DisplayName) : ErrorMessage,
                ValidationType = "enforcetrue"
            };
        }
    }
}