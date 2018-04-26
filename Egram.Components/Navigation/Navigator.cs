using System;
using System.Reactive.Linq;

namespace Egram.Components.Navigation
{
    public class Navigator
    {
        private readonly object _locker = new object();

        private Board _board;
        private event EventHandler<Board> BoardChanged;
        public IObservable<Board> BoardNavigations() => Observable.FromEventPattern<Board>(
            h => BoardChanged += h,
            h => BoardChanged -= h)
                .Select(args => args.EventArgs);

        private Scope _scope;
        private event EventHandler<Scope> ScopeChanged;
        public IObservable<Scope> ScopeNavigations() => Observable.FromEventPattern<Scope>(
                h => ScopeChanged += h,
                h => ScopeChanged -= h)
            .Select(args => args.EventArgs);

        private Topic _topic;
        private event EventHandler<Topic> TopicChanged;
        public IObservable<Topic> TopicNavigations() => Observable.FromEventPattern<Topic>(
                h => TopicChanged += h,
                h => TopicChanged -= h)
            .Select(args => args.EventArgs);

        private Workarea _workarea;
        private event EventHandler<Workarea> WorkareaChanged; 
        public IObservable<Workarea> WorkareaNavigations() => Observable.FromEventPattern<Workarea>(
                h => WorkareaChanged += h,
                h => WorkareaChanged -= h)
            .Select(args => args.EventArgs);
        
        public void Go(Board board)
        {
            lock (_locker)
            {
                if (_board != board)
                {
                    _board = board;
                    BoardChanged?.Invoke(this, board);
                }
            }
        }

        public void Go(Scope scope)
        {
            lock (_locker)
            {
                if (_scope != scope)
                {
                    _scope = scope;
                    ScopeChanged?.Invoke(this, scope);
                }
            }
        }

        public void Go(Topic topic)
        {
            lock (_locker)
            {
                if (_topic != topic)
                {
                    _topic = topic;
                    TopicChanged?.Invoke(this, topic);
                }
            }
        }

        public void Go(Workarea workarea)
        {
            lock (_locker)
            {
                if (_workarea != workarea)
                {
                    _workarea = workarea;
                    WorkareaChanged?.Invoke(this, workarea);
                }
            }
        }
    }
}