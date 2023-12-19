using EF_Test.Base;
using EF_Test.Entity;
using System.ComponentModel.DataAnnotations;

namespace EF_Test.ViewModel
{
    public record RealizedPlViewModel : IViewModel<RealizedPlEntity>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double RealizedPL { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        //[Required]
        //public DateTime CreateDate { get; set; }

        //public RealizedPlEntity MapToEntity()
        //{
        //    return new RealizedPlEntity()
        //    {
        //        Name = Name,
        //        RealizedPL = RealizedPL,
        //        CreateDate = CreateDate.ToString("yyyyMMdd"),
        //    };
        //}

        //public RealizedPlEntity MapToEntity()
        //{
        //    return new RealizedPlEntity()
        //    {
        //        Name = Name,
        //        RealizedPL = RealizedPL,
        //        CreateDate = CreateDate,
        //    };
        //}

        public RealizedPlEntity MapToEntity()
        {
            return new RealizedPlEntity()
            {
                Name = Name,
                RealizedPL = RealizedPL,
                CreateDate = CreateDate.ToString("yyyy-MM-dd HH:mm"),
            };
        }
    }
}
