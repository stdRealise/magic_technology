using Godot;
using MagicTechnology.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
            var bulletScene = GD.Load<PackedScene>("res://scenes/environments/Bullet.tscn");
            var b = bulletScene.Instantiate<Bullet>();

            b.Speed = 10f;
            Camera3D camera = info.player.GetNode<Camera3D>("Head/Camera3D");
            b.Direction = -camera.GlobalTransform.Basis.Z;
            b.Position = info.player.Position;
            info.player.GetTree().CurrentScene.AddChild(b);
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
        }

        private void RegisterElements()
        {
            _spellsElements.Add(SpellCategory.None, null);
            _spellsElements.Add(SpellCategory.Wet, new BaseSpellElement());
            _spellsElements.Add(SpellCategory.Ignitability, new BaseSpellElement());
            _spellsElements.Add(SpellCategory.Stun, new BaseSpellElement());
            _spellsElements.Add(SpellCategory.Cold, new BaseSpellElement());
            _spellsElements.Add(SpellCategory.ElectoShock, new BaseSpellElement());
            _spellsElements.Add(SpellCategory.Ice, new BaseSpellElement());
            _spellsElements.Add(SpellCategory.Test, new TestSpellElement());
        }

        private void RegisterSpells()
        {
            _spells.Add("TestSpell", new Spell("TestSpell", 1, 0, SpellCategory.Test));
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
                GD.Print("SpellManager: coudnt find spell ", name);
            }
        }
    }
}
