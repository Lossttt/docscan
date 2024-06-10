using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace document_scanner.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Het bestandsnaam is vereist.")]
        [StringLength(255, ErrorMessage = "De bestandsnaam mag niet langer zijn dan 255 tekens.")]
        public required string FileName { get; set; }

        [Required(ErrorMessage = "De originele bestandsnaam is vereist.")]
        [StringLength(255, ErrorMessage = "De originele bestandsnaam mag niet langer zijn dan 255 tekens.")]
        public required string OriginalFileName { get; set; }

        [Required(ErrorMessage = "De bestandsgegevens zijn vereist.")]
        public required byte[] FileData { get; set; }

        [StringLength(100, ErrorMessage = "De content type mag niet langer zijn dan 100 tekens.")]
        public required string ContentType { get; set; }

        [Required(ErrorMessage = "De upload datum is vereist.")]
        [DataType(DataType.DateTime, ErrorMessage = "De upload datum moet een geldige datum en tijd bevatten.")]
        public DateTime UploadDate { get; set; }
    }
}
