﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic;
using kinnemed05.Models;
using System.Web.Mvc;

namespace kinnemed05.DataAnnotation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false,Inherited=true)]
    public class Unique : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            using (bd_kinnemed02Entities db = new bd_kinnemed02Entities())
            {
                var Name = validationContext.MemberName;

                if (string.IsNullOrEmpty(Name))
                {
                    var displayName = validationContext.DisplayName;

                    var prop = validationContext.ObjectInstance.GetType().GetProperty(displayName);

                    if (prop != null)
                    {
                        Name = prop.Name;
                    }
                    else
                    {
                        var props = validationContext.ObjectInstance.GetType().GetProperties().Where(x => x.CustomAttributes.Count(a => a.AttributeType == typeof(DisplayAttribute)) > 0).ToList();

                        foreach (PropertyInfo prp in props)
                        {
                            var attr = prp.CustomAttributes.FirstOrDefault(p => p.AttributeType == typeof(DisplayAttribute));

                            var val = attr.NamedArguments.FirstOrDefault(p => p.MemberName == "Name").TypedValue.Value;

                            if (val.Equals(displayName))
                            {
                                Name = prp.Name;
                                break;
                            }
                        }
                    }
                }

                PropertyInfo IdProp = validationContext.ObjectInstance.GetType().GetProperties().FirstOrDefault(x => x.CustomAttributes.Count(a => a.AttributeType == typeof(KeyAttribute)) > 0);

                int Id = (int)IdProp.GetValue(validationContext.ObjectInstance, null);

                Type entityType = validationContext.ObjectType;


                var result = db.Set(entityType).Where(Name + "==@0", value);

                int count = 0;

                if (Id > 0)
                {
                    result = result.Where(IdProp.Name + "<>@0", Id);
                }

                count = result.Count();

                if (count == 0)
                    return ValidationResult.Success;
                else
                    return new ValidationResult(ErrorMessageString);
            }


        }
    }



    class IsUnique : ValidationAttribute
    {
        public IsUnique(string propertyNames)
        {
            this.FieldName = propertyNames;
        }

        public string FieldName { get; private set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var myproperty = validationContext.ObjectType.GetProperty(FieldName);
            //var value = myproperty.GetValue(validationContext.ObjectInstance, null);

            //IEnumerable<String> properties;

            List<string> propertiesList = new List<string>();
            propertiesList.Add(myproperty.Name);

            var dba = new bd_kinnemed02Entities();
            Type entityType = validationContext.ObjectType;
            //var result=dba.doctor.Where(FieldName + "==@0", value);
            var result = dba.Set(entityType).Where(FieldName + "==@0", value);
            //if (dba.doctor.Any(article => article.doc_cedula == (string)value))
            if (result.Count()>0)
            {
                return new ValidationResult("La cédula ya existe" + validationContext.MemberName, propertiesList);
            }
            return null;
        }
    }
}