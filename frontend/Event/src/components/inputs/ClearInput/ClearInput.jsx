import styles from "./ClearInput.module.css";

const ClearInput = ({
    value, 
    setValue, 
    typeInput = "text", 
    placeHolder = "",
    name = ""}) => {
  return (
    <div className={styles.ClearInput__Main}>
      <input className={styles.ClearInput__Input}
        type={typeInput} 
        name={name}
        value={value}
        onChange={setValue}
        placeholder={placeHolder}
        />
    </div>
  )
}

export default ClearInput;