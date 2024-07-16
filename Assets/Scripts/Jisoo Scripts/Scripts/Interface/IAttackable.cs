public interface IAttackable
{
    int damage { get; set; }
    float range { get; set; }

    float attackSpeed { get; set; }


    void SendDamage(int damage);
}
