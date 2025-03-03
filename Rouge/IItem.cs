﻿namespace Rouge
{
    internal interface IItem
    {
        string Name { get; }
        void ApplyEffect(Player player);
    }
}