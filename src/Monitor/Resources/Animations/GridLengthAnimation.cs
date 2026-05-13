using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Monitor.Resources.Animations
{
    public class GridLengthAnimation : AnimationTimeline
    {

        //Animacion para Collapse gradual de menu Vertical ===========================================

        public override Type TargetPropertyType => typeof(GridLength);

        // FROM
        public GridLength From
        {
            get { return (GridLength)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register(
                "From",
                typeof(GridLength),
                typeof(GridLengthAnimation)
            );

        //  TO
        public GridLength To
        {
            get { return (GridLength)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register(
                "To",
                typeof(GridLength),
                typeof(GridLengthAnimation)
            );

        //  EASING 
        public IEasingFunction EasingFunction { get; set; }

        //  CÁLCULO DE ANIMACIÓN
        public override object GetCurrentValue(
            object defaultOriginValue,
            object defaultDestinationValue,
            AnimationClock animationClock)
        {
            double from = From.Value;
            double to = To.Value;

            double progress = animationClock.CurrentProgress.Value;

            //  aplicar easing si existe
            if (EasingFunction != null)
            {
                progress = EasingFunction.Ease(progress);
            }

            double current = (to - from) * progress + from;

            return new GridLength(current, GridUnitType.Pixel);
        }

        //  requerido por WPF
        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }




    }


}
