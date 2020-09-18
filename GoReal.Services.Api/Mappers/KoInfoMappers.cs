using GoReal.Models.Entities;

namespace GoReal.Services.Api.Mappers
{
    public static class KoInfoMappers
    {
        public static Stone ToStone(this string entity)
        {
            string[] stone = entity.Split(',');
            bool? color;

            if (bool.TryParse(stone[2], out bool value))
                color = value;
            else
                color = null;

            return new Stone()
            {
                Row = int.Parse(stone[0]),
                Column = int.Parse(stone[1]),
                Color = color
            };
        }

        public static string ToDal(this Stone entity)
        {
            return $"{entity.Row},{entity.Column},{(entity.Color.HasValue ?entity.Color.ToString() : "null")}";
        }
    }
}
