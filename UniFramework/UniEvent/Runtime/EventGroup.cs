using System;
using System.Collections;
using System.Collections.Generic;

namespace Uni.Event
{
    public class EventGroup
    {
        private readonly Dictionary<int, List<Action<IEventMessage>>> _cachedListener = new Dictionary<int, List<Action<IEventMessage>>>();

        /// <summary>
        /// 添加一个监听
        /// </summary>
        public void AddListener(int eventId, System.Action<IEventMessage> listener)
        {
            if (_cachedListener.ContainsKey(eventId) == false)
                _cachedListener.Add(eventId, new List<Action<IEventMessage>>());

            if (_cachedListener[eventId].Contains(listener) == false)
            {
                _cachedListener[eventId].Add(listener);
                UniEvent.AddListener(eventId, listener);
            }
            else
            {
                UniLogger.Warning($"Event listener is exist : {eventId}");
            }
        }

        /// <summary>
        /// 添加一个监听
        /// </summary>
        public void AddListener<TEvent>(System.Action<IEventMessage> listener) where TEvent : IEventMessage
        {
            AddListener(typeof(TEvent).GetHashCode(), listener);
        }

        public void RemoveListener(int eventId, System.Action<IEventMessage> listener)
        {
            if (!_cachedListener.ContainsKey(eventId)) return;

            if (_cachedListener[eventId].Contains(listener))
            {
                _cachedListener[eventId].Remove(listener);
                UniEvent.RemoveListener(eventId, listener);
            }

            if (_cachedListener[eventId].Count == 0) _cachedListener.Remove(eventId);
        }

        /// <summary>
        /// 移除所有缓存的监听
        /// </summary>
        public void RemoveAllListener()
        {
            foreach (var pair in _cachedListener)
            {
                int eventId = pair.Key;
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    UniEvent.RemoveListener(eventId, pair.Value[i]);
                }
                pair.Value.Clear();
            }
            _cachedListener.Clear();
        }
    }
}
