namespace GuildMaster.Tools
{
    /// <summary>
    /// explicit boxing.
    /// </summary>
    /// <typeparam name="T"> Boxed value type </typeparam>
    public class Box<T> where T: struct
    {
        public Box(T value)
        {
            Value = value;
        }
        public T Value;

        public override string ToString() => Value.ToString();

        public static implicit operator Box<T>(T val) => new Box<T>(val);
    }
}