namespace ECS.Game.Components.TheDeeperComponent
{
    public struct WallComponent
    {
        public WallColor color;
        public enum WallColor
        {
            Red,
            Blue,
            Yellow,
            Dark,
            Green,
            Default
        }
        //если добавить none - все сломается ( почему??? ) 
    }
}