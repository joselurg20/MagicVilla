using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API.Datos
{
    public class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto> {

            new VillaDto {Id=1,Name="vista a la piscina", Ocupantes=5, MetrosCuadrados=50},
            new VillaDto {Id=2,Name="vista al campo", Ocupantes=4, MetrosCuadrados=40},
            new VillaDto {Id=3,Name="vista al mar", Ocupantes=3, MetrosCuadrados=30}
            
        };
    }
}
