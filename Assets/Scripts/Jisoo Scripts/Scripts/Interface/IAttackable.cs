public interface IAttackable
{
    int damage { get; set; }
    float range { get; set; }

    void SendDamage(int damage);
}
