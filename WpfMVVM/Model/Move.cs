namespace WpfMVVM.Model
{
    public class Move
    {
        public Cell From { get; }

        public Cell To { get; }

        public Move(Cell from, Cell to)
        {
            From = from;
            To = to;
        }
    }
}
