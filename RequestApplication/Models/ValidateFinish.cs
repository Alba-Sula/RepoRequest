using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RequestApplication.Models
{
    public class ValidateFinish : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var request = (RequestTable)validationContext.ObjectInstance;
            if (request.Status.Title == "Finished")
            {
                if (request.RequestFinished == null)
                {
                    return new ValidationResult("Finished date is required when status is finished");
                }
            }
            else
            {
                request.RequestFinished = null;
                return ValidationResult.Success;
            }
            return ValidationResult.Success;
        }
    }
}