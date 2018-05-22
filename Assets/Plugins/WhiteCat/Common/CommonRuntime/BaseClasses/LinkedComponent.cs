using System;

namespace WhiteCat
{
    /// <summary>
    /// LinkedComponent
    /// </summary>
    public class LinkedComponent<T> : ScriptableComponentWithEditor where T : LinkedComponent<T>
    {
        static T _first;
        static T _last;

        T _this;

        T _prev;
        T _next;


        public LinkedComponent()
        {
            _this = this as T;
        }


        public static T listFirst
        {
            get { return _first; }
        }


        public static T listLast
        {
            get { return _last; }
        }


        public static bool isListEmpty
        {
            get { return !_first; }
        }


        public T listPrevious
        {
            get { return _prev; }
        }


        public T listNext
        {
            get { return _next; }
        }


        protected void AttachAsListLast()
        {
#if DEBUG
            if (_prev || _next || _first == _this) throw new Exception("Node already in Linked List");
#endif

            _prev = _last;
            if (_last) _last._next = _this;
            else _first = _this;
            _last = _this;
        }


        protected void AttachAsListFirst()
        {
#if DEBUG
            if (_prev || _next || _first == _this) throw new Exception("Node is Already in Linked List");
#endif

            _next = _first;
            if (_first) _first._prev = _this;
            else _last = _this;
            _first = _this;
        }


        protected void AttachBefore(T target)
        {
#if DEBUG
            if (_prev || _next || _first == _this) throw new Exception("Node is Already in Linked List");
            if (!target._prev && !target._next && _first != target) throw new Exception("Target Node is Not in Linked List");
#endif

            _next = target;
            _prev = target._prev;
            target._prev = _this;
            if (_first == target) _first = _this;
        }


        protected void AttachAfter(T target)
        {
#if DEBUG
            if (_prev || _next || _first == _this) throw new Exception("Node is Already in Linked List");
            if (!target._prev && !target._next && _first != target) throw new Exception("Target Node is Not in Linked List");
#endif

            _prev = target;
            _next = target._next;
            target._next = _this;
            if (_last == target) _last = _this;
        }


        protected void DetachFromList()
        {
#if DEBUG
            if (!_prev && !_next && _first != _this) throw new Exception("Node is Not in Linked List");
#endif

            if (_first == _this) _first = _next;
            if (_last == _this) _last = _prev;
            if (_prev) _prev._next = _next;
            if (_next) _next._prev = _prev;
            _prev = null;
            _next = null;
        }

    } // class LinkedComponent

} // namespace WhiteCat