using Godot;
using MagicTechnology.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MagicTechology
{
	public enum SpellCategory
	{
		None,
		Wet,
		Ignitability,
		Stun,
		Cold,
		ElectoShock,
		Ice,
		Test
	}

	public class BaseSpellElement : ISpellElement
	{
		public BaseSpellElement() { }
		public void Cast(ref SpellCastInfo info)
		{
			throw new NotImplementedException();
		}
	}

	public class TestSpellElement : ISpellElement
	{
		public TestSpellElement() { }

		public void Cast(ref SpellCastInfo info)
		{
			//test
			var bulletScene = GD.Load<PackedScene>("res://scenes/environments/PhisicalBullet.tscn");
			var b = bulletScene.Instantiate<PhisicalBullet>();

			b.Speed = 10f;
			b.Damage = 25f;
			Camera3D camera = info.player.GetNode<Camera3D>("Head/Camera3D");
			Node3D rightHand = info.player.GetNode<Node3D>("Head/Camera3D/RightHand");
			b.Direction = -camera.GlobalTransform.Basis.Z;
			info.player.GetTree().CurrentScene.AddChild(b);
			b.AddCollisionExceptionWith(info.player);
			b.GlobalPosition = rightHand.GlobalPosition;
			b.LookAt(b.GlobalPosition + b.Direction, Vector3.Up);
			//end test
		}
	}

    public class ElectoShockSpellElement : ISpellElement
    {
        public ElectoShockSpellElement() { }

        public void Cast(ref SpellCastInfo info)
        {
            //test
            var bulletScene = GD.Load<PackedScene>("res://scenes/environments/PhisicalBullet.tscn");
            var b = bulletScene.Instantiate<PhisicalBullet>();

            b.Speed = 10f;
            b.Damage = 25f;
            Camera3D camera = info.player.GetNode<Camera3D>("Head/Camera3D");
            Node3D rightHand = info.player.GetNode<Node3D>("Head/Camera3D/RightHand");
            b.Direction = -camera.GlobalTransform.Basis.Z;
            info.player.GetTree().CurrentScene.AddChild(b);
            b.AddCollisionExceptionWith(info.player);
            b.GlobalPosition = rightHand.GlobalPosition;
            b.LookAt(b.GlobalPosition + b.Direction, Vector3.Up);
            //end test
        }
    }

    public class Spell
	{
		private string _name;
		private int _cost;
		private int _manaCost;
		private SpellCategory _spellCategory;

		public Spell(string name, int cost, int manaCost, SpellCategory spellCategory)
		{
			_name = name;
			_cost = cost;
			_manaCost = manaCost;
			_spellCategory = spellCategory;
		}

		public SpellCategory GetCategory() { return _spellCategory;  }   
	}

	public class SpellManager
	{
		static private SpellManager _instance = null;
		static public SpellManager GetInstance()
		{
			if (_instance == null)
				_instance = new SpellManager();

			return _instance;
		}

		private Dictionary<SpellCategory, ISpellElement> _spellsElements = new Dictionary<SpellCategory, ISpellElement>();
		private Dictionary<string, Spell> _spells = new Dictionary<string, Spell>();

		public SpellManager()
		{
			RegisterElements();
			RegisterSpells();

			LoadFromFile("res://spells.txt");
		}

		private void RegisterElements()
		{
			_spellsElements.Add(SpellCategory.None, null);
			_spellsElements.Add(SpellCategory.Wet, new BaseSpellElement());
			_spellsElements.Add(SpellCategory.Ignitability, new BaseSpellElement());
			_spellsElements.Add(SpellCategory.Stun, new BaseSpellElement());
			_spellsElements.Add(SpellCategory.Cold, new BaseSpellElement());
			_spellsElements.Add(SpellCategory.ElectoShock, new ElectoShockSpellElement());
			_spellsElements.Add(SpellCategory.Ice, new BaseSpellElement());
			_spellsElements.Add(SpellCategory.Test, new TestSpellElement());
		}

		private void RegisterSpells()
		{
		//	_spells.Add("TestSpell", new Spell("TestSpell", 1, 0, SpellCategory.Test));
		}

		private void LoadFromFile(string path)
		{
			GD.Print("SpellManager: loading ", path);

			// Read file
			using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);

			// Get whole file as string
			string content = file.GetAsText();

			// Parse file
			using (System.IO.StringReader reader = new System.IO.StringReader(content))
			{
				string? line;
				while ((line = reader.ReadLine()) != null)
				{
					string trimString = line.Trim();

					// Skip commentary
					if (trimString.Length > 0 && trimString[0] == '#')
						continue;

					string[] parts = line.Split(' ');

					// 5 частей
					if (parts.Length == 5 && parts[0] == "Spell")
					{
						string name = parts[1];
						string cost = parts[2];
						string manaCost = parts[3];
						string category = parts[4];

						_spells.Add(name, new Spell(name, cost.ToInt(), manaCost.ToInt(), GetSpellCategory(category)));

						GD.Print("SpellManager: Added spell ", name, " cost ", cost, " manaCost ", manaCost, " category ", category);
					}
				}
			}
		}

		public void CastSpell(Player player, string name)
		{
			_spells.TryGetValue(name, out Spell spell);
			if (spell != null)
			{
				SpellCategory spellCategory = spell.GetCategory();
				_spellsElements.TryGetValue(spellCategory, out ISpellElement spellElement);
				if (spellElement != null)
				{
					SpellCastInfo castInfo = new SpellCastInfo();
					castInfo.Name = name;
					castInfo.player = player;
					spellElement.Cast(ref castInfo);
				}
			}
			else
			{
				GD.Print("SpellManager: couldn't find spell ", name);
			}
		}

        static Dictionary<string, SpellCategory> _categoryPairs = null;
        static SpellCategory GetSpellCategory(string name)
		{
			// lazy initialization
			if (_categoryPairs == null)
			{
                _categoryPairs = new Dictionary<string, SpellCategory>();

				//_categoryPairs.Add("None", SpellCategory.None);
				_categoryPairs.Add("Wet", SpellCategory.Wet);
				_categoryPairs.Add("Ignitability", SpellCategory.Ignitability);
				_categoryPairs.Add("Stun", SpellCategory.Stun);
				_categoryPairs.Add("Cold", SpellCategory.Cold);
				_categoryPairs.Add("ElectoShock", SpellCategory.ElectoShock);
				_categoryPairs.Add("Ice", SpellCategory.Ice);
				_categoryPairs.Add("Test", SpellCategory.Test);
            }

            if (_categoryPairs.TryGetValue(name, out SpellCategory spellCategory))
				return spellCategory;

            GD.Print("SpellManager: unknown category ", name);
			return SpellCategory.None;
		}
	}
}
