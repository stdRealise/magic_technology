
using Godot;

namespace MagicTechnology.Interfaces
{
	/// <summary>
	/// Interface for all interactive objects in the game.
	/// Enables a unified mechanism for player interaction with the environment.
	/// </summary>
	public interface IInteractable
	{
		/// <summary>
		/// Called when the player interacts with the object.
		/// </summary>
		/// <param name="player">Reference to the player object that initiated the interaction</param>
		void Interact(Player player);
	}

	public struct SpellCastInfo
	{
		public string Name;
		public string Description;
		public Player player;
	}

	public interface ISpellElement
	{
		void Cast(ref SpellCastInfo info);
	}

	public interface IWeapon
	{
		void Cast(Player player, SpellCastInfo info, ISpellElement spellElement);
	}
}
