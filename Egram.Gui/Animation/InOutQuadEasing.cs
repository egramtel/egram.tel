using Avalonia.Animation;

namespace Egram.Gui.Animation
{
    public class InOutQuadEasing : IEasing<double>
    {
        public double Ease(double progress, double start, double finish)
        {
            progress = progress < .5 ? 2 * progress * progress : -1 + (4 - 2 * progress) * progress;
            return (finish - start) * progress + start;
        }

        public object Ease(double progress, object start, object finish)
        {
            return Ease(progress, (double) start, (double) finish);
        }
    }
}