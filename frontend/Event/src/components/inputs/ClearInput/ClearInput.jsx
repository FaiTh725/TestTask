import styles from "./ClearInput.module.css";

const ClearInput = ({
    value, 
    setValue, 
    typeInput = "text", 
    placeHolder = ""}) => {
  return (
    <div className={styles.ClearInput__Main}>
      <input className={styles.ClearInput__Input}
        type={typeInput} 
        value={value}
        onChange={setValue}
        placeholder={placeHolder}
        />
    </div>
  )
}

export default ClearInput;