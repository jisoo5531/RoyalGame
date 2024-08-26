public interface IDamagable
{
    int HP { get; set; }
    int maxHP { get; set; }
    void GetDamage(int damage);    
}
