
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
}
