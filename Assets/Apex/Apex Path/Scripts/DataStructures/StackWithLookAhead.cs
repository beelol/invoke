﻿/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.DataStructures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A stack ADT that supports look ahead.
    /// </summary>
    /// <typeparam name="T">The item type</typeparam>
    public class StackWithLookAhead<T> : IIterable<T>
    {
        private T[] _array;
        private int _used;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackWithLookAhead{T}"/> class.
        /// </summary>
        public StackWithLookAhead()
            : this(4)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StackWithLookAhead{T}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public StackWithLookAhead(int capacity)
        {
            capacity = Math.Max(capacity, 4);
            _array = new T[capacity];
            _used = 0;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int count
        {
            get { return _used; }
        }

        /// <summary>
        /// Gets the item with the specified index.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        /// <param name="idx">The index.</param>
        /// <returns>The item at the specified index</returns>
        public T this[int idx]
        {
            get
            {
                if (idx < 0 || idx >= _used)
                {
                    throw new ArgumentOutOfRangeException("idx", "The stack does not contain an item at that index.");
                }

                idx = (_used - 1) - idx;
                return _array[idx];
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            Array.Clear(_array, 0, _used);
            _used = 0;
        }

        /// <summary>
        /// Looks the ahead a maximum of <paramref name="steps"/>. If there are less items on the stack it will just return those items.
        /// </summary>
        /// <param name="steps">The steps.</param>
        /// <returns>The items from the top to <paramref name="steps"/> deep.</returns>
        public IEnumerator<T> LookAhead(int steps)
        {
            steps = Math.Min(steps, _used);

            for (int i = 0; i < steps; i++)
            {
                yield return _array[i];
            }
        }

        /// <summary>
        /// Returns the item at the top of the stack, but does not remove it.
        /// </summary>
        /// <returns>The item at the top of the stack.</returns>
        /// <exception cref="System.InvalidOperationException">The stack is empty.</exception>
        public T Peek()
        {
            if (_used == 0)
            {
                throw new InvalidOperationException("The stack is empty.");
            }

            return _array[_used - 1];
        }

        /// <summary>
        /// Returns the item at index <paramref name="index"/> as seen from the top of the stack.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The item at index <paramref name="index"/> as seen from the top of the stack.</returns>
        /// <exception cref="System.InvalidOperationException">The stack does not contain an item at that index.</exception>
        public T PeekFront(int index)
        {
            return PeekBack(_used - 1 - index);
        }

        /// <summary>
        /// Returns the item at index <paramref name="index"/> as seen from the bottom of the stack.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The item at index <paramref name="index"/> as seen from the bottom of the stack.</returns>
        /// <exception cref="System.InvalidOperationException">The stack does not contain an item at that index.</exception>
        public T PeekBack(int index)
        {
            if (index > _used - 1 || index < 0)
            {
                throw new ArgumentOutOfRangeException("idx", "The stack does not contain an item at that index.");
            }

            return _array[index];
        }

        /// <summary>
        /// Returns the item at the bottom of stack.
        /// </summary>
        /// <returns>The item at the bottom of stack.</returns>
        /// <exception cref="System.InvalidOperationException">The stack is empty.</exception>
        public T Last()
        {
            if (_used == 0)
            {
                throw new InvalidOperationException("The stack is empty.");
            }

            return _array[0];
        }

        /// <summary>
        /// Removes and returns the item at the top of the stack.
        /// </summary>
        /// <returns>The item at the top of the stack</returns>
        /// <exception cref="System.InvalidOperationException">The stack is empty.</exception>
        public T Pop()
        {
            if (_used == 0)
            {
                throw new InvalidOperationException("The stack is empty.");
            }

            _used--;

            T t = _array[_used];
            _array[_used] = default(T);

            return t;
        }

        /// <summary>
        /// Pushes the specified item onto the stack.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Push(T item)
        {
            if (_used == _array.Length)
            {
                var newArray = new T[2 * _array.Length];
                Array.Copy(_array, 0, newArray, 0, _used);
                _array = newArray;
            }

            _array[_used++] = item;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            for (int i = _used - 1; i >= 0; i--)
            {
                yield return _array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = _used - 1; i >= 0; i--)
            {
                yield return _array[i];
            }
        }
    }
}
