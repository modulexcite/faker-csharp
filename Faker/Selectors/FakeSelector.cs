﻿using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Faker.Selectors
{
    /// <summary>
    ///     <see cref="ITypeSelector" /> that uses an underlying <see cref="Fake{T}" />
    ///     to generate instances of type <see cref="T" />. Designed to allow composition
    ///     of fakes for complex classes.
    /// </summary>
    public sealed class FakeSelector : ITypeSelector
    {
        private readonly IFake _internalFake;

        public FakeSelector(IFake internalFake)
        {
            Contract.Requires(internalFake != null);
            _internalFake = internalFake;
        }

        /// <summary>
        /// Always <see cref="SelectorPriorityConstants.CustomSelectorPriorty"/> by default.
        /// </summary>
        public int Priority { get; set; }

        public Type TargetType => _internalFake.SupportedType;
        private bool _can_be_null;
        public void BeNull(bool canBeNull = false)
        {
            _can_be_null = canBeNull;
        }

        public bool CanBind(PropertyInfo field)
        {
            return CanBind(field.PropertyType);
        }

        public bool CanBind(Type type)
        {
            return type.IsAssignableFrom(TargetType);
        }

        public void Generate(object targetObject, PropertyInfo property)
        {
            property.SetValue(targetObject, _internalFake.Generate(), null);
        }

        public object Generate(ref object targetObject)
        {
            targetObject = _internalFake.Generate();
            return targetObject;
        }
    }
}