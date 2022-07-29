using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vbSparkle.Web.Models
{
    public class CodeUploadModel
    {
        [Required]
        public string Before { get; set; }

        public string After { get; set; }
    }
}
