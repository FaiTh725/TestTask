import styles from "./PrimaryButton.module.css";

const PrimaryButton = ({text, action}) => {
  return (
    <div className={styles.PrimaryButton__Main}>
      <button className={styles.PrimaryButton__Button}
        onClick={action}>
        {text}
      </button>
    </div>
  )
}

export default PrimaryButton;