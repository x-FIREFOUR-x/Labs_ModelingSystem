namespace Lab2
{
    public enum Distribution
    {
        Exponential,
        Normal,
        Uniform,
        Constant
    }

    public class DelayGenerator
    {
        private Distribution _distribution;

        public DelayGenerator(Distribution distribution)
        {
            _distribution = distribution;
        }

        public double GetDelay(double averageDelay)
        {
            switch (_distribution)
            {
                case Distribution.Exponential:
                    return averageDelay;

                case Distribution.Normal:
                    return averageDelay;

                case Distribution.Uniform:
                    return averageDelay;

                case Distribution.Constant:
                    return averageDelay;

                default:
                    return averageDelay;
            }
        }
    }
}
