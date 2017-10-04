using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TelerikMovies.Models.Contracts;

namespace TelerikMovies.Models.Abstract
{
    public abstract class BaseInfoModel : IAuditable, IDeletable
    {
        public BaseInfoModel()
        {
            this.Id = Guid.NewGuid();
            this.IsDeleted = false;
        }

        [Key]
        public Guid Id { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }
    }
}
