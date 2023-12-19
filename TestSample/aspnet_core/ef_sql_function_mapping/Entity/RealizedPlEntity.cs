using EF_Test.Base;
using System.ComponentModel.DataAnnotations;

namespace EF_Test.Entity
{
    public record RealizedPlEntity : IEntity
    {
        [Key]
        public long No { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public double RealizedPL { get; set; }

        /// <summary>
        /// 문자 -> 날짜 변환 목표
        /// </summary>
        [Required]
        public string CreateDate { get; set; }


        /// OnModelCreating.HasConversion() 사용시
        //[Required]
        //public DateOnly CreateDate { get; set; }
    }
}
