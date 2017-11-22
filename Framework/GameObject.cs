﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Framework.Object;

namespace Framework {

	// TODO Add static caches in a list with specific types (collision components, ...)
	// to improve performance
	// To achieve this add a IsAlive property

	public class GameObject {

		public Lifecycle Lifecycle { get; } = new Lifecycle();

		public Transform Transform { get; }

		// General properties
		public bool IsEnabled { get; set; } = true;
		public bool IsUiElement { get; set; }

		// Parent and children
		public GameObject Parent { get; private set; }
		private readonly List<GameObject> children = new List<GameObject>();
		public ReadOnlyCollection<GameObject> Children { get; }

		// Components
		private readonly List<Component> components = new List<Component>();
		public ReadOnlyCollection<Component> Components { get; }


		public GameObject(bool isUiElement = false) {
			Transform = new Transform {GameObject = this};
			Children = new ReadOnlyCollection<GameObject>(children);
			Components = new ReadOnlyCollection<Component>(components);
			IsUiElement = isUiElement;

			// Register lifecycle delegation for components
			Lifecycle.onDestroy += () => components.ForEach(c => c.Lifecycle?.onDestroy?.Invoke());
		}

		// TODO GetChild<Type> ?
		// TODO GetChildAt(int index)

		public void AddChild(GameObject child) {
			child.Parent = this;
			child.Lifecycle?.onCreate?.Invoke();
			children.Add(child);
		}

		public void RemoveChild(GameObject child) {
			child.Lifecycle?.onDestroy?.Invoke();
			children.Remove(child);
		}

		public TComponentType GetComponent<TComponentType>() {
			try {
				var component = components.First(c => c is TComponentType);
				return (TComponentType) (object) component;
			} catch (InvalidOperationException) {
				return default(TComponentType);
			}
		}

		public List<TComponentType> GetComponents<TComponentType>() {
			var castedComponents = new List<TComponentType>();
			components.ForEach(c => {
				if (c is TComponentType) {
					castedComponents.Add((TComponentType) (object) c);
				}
			});
			return castedComponents;
		}

		public void AddComponent(Component component) {
			component.GameObject = this;
			component.Lifecycle?.onCreate?.Invoke();
			components.Add(component);
		}

		public void RemoveComponent(Component component) {
			component.Lifecycle?.onDestroy?.Invoke();
			components.Remove(component);
		}

		public virtual void Update() {
			children.ForEach(go => {
				// Skip disabled gameobjects
				if (!go.IsEnabled) {
					return;
				}
				go.Update();
			});
			components.ForEach(c => {
				// Skip disabled components
				if (!c.IsEnabled) {
					return;
				}
				if (c is UpdateComponent updateComponent) {
					updateComponent.Update();
				}
			});
		}

		public virtual void Render() {
			// Invalidate the transforms caches to not draw the same stuff like the last frame
			// and so be able to have a cache
			Transform.Invalidate();

			children.ForEach(go => {
				// Skip disabled gameobjects
				if (!go.IsEnabled) {
					return;
				}
				go.Render();
			});
			components.ForEach(c => {
				// Skip disabled components
				if (!c.IsEnabled) {
					return;
				}
				if (c is RenderComponent renderComponent) {
					renderComponent.Render();
				}
			});
		}
	}

}